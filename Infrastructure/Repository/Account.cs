﻿using Application.DTO.Request.ActivityTracker;
using Application.DTO.Request.Identity;
using Application.DTO.Response;
using Application.DTO.Response.ActivityTracker;
using Application.DTO.Response.Identity;
using Application.Extension.Identity;
using Infrastructure.DataAccess;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Application.Interface.Identity;
using Domain.Entities.ActivityTracker;

namespace Infrastructure.Repository
{
    public class Account
        (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context) : Application.Interface.Identity.IAccount
    {
        public string Username => throw new NotImplementedException();

        public string Environment => throw new NotImplementedException();

        public AccountId HomeAccountId => throw new NotImplementedException();

        public async Task<ServiceResponse> CreateUserAsync(CreateUserRequestDTO model)
        {
            var user = await FindUserByEmail(model.Email);
            if (user != null)
                return new ServiceResponse(false, "User already exist");
            var newUser = new ApplicationUser()
            {
                UserName = model.Email,
                PasswordHash = model.Password,
                Email = model.Email,
                Name = model.Name
            };
            var result = CheckResult(await userManager.CreateAsync(newUser, model.Password));
            if (!result.Flag)
                return result;
            else
                return await CreateUserClaims(model);

        }
        private async Task<ServiceResponse> CreateUserClaims(CreateUserRequestDTO model)
        {
            if (string.IsNullOrEmpty(model.Policy)) return new ServiceResponse(false, "No policy specified");
            Claim[] userClaims = [];
            if (model.Policy.Equals(Policy.AdminPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims =
                    [
                        new Claim(ClaimTypes.Email, model.Email),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim("Name", model.Name),
                        new Claim("Create", "true"),
                        new Claim("Update", "true"),
                        new Claim("Delete", "true"),
                        new Claim("Read", "true"),
                        new Claim("ManageUser", "true")
                    ];
            }
            else if (model.Policy.Equals(Policy.ManagerPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims = [
                     new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "Manager"),
                    new Claim("Name", model.Name),
                    new Claim("Create", "true"),
                    new Claim("Update", "true"),
                    new Claim("Delete", "true"),
                    new Claim("Read", "true"),
                    new Claim("ManageUser", "false")

                ];
            }
            else if (model.Policy.Equals(Policy.UserPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims = [
                     new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim("Name", model.Name),
                    new Claim("Create", "false"),
                    new Claim("Update", "false"),
                    new Claim("Delete", "false"),
                    new Claim("Read", "false"),
                    new Claim("ManageUser", "false")

                ];

            }
                var result = CheckResult(await userManager.AddClaimsAsync((await FindUserByEmail(model.Email)), userClaims));
                if (result.Flag)
                    return new ServiceResponse(true, "User created");
                else
                    return result;

            }
        public async Task<ServiceResponse> LoginAsync(LoginUserRequestDTO model)
            {
                var user = await FindUserByEmail(model.Email);
                if (user is null) return new ServiceResponse(false, "User not found");

                var verifyPassword = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!verifyPassword.Succeeded)
                return new ServiceResponse(false, "Incorrect credentials provided");

                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (!result.Succeeded)
                    return new ServiceResponse(false, "Unknown error occured while logging you in");
                else
                    return new ServiceResponse(true, null);
            }
            private  async Task<ApplicationUser> FindUserByEmail(string email)
                 => await userManager.FindByEmailAsync(email);

            private async  Task<ApplicationUser> FindUserById(string id) => await userManager.FindByIdAsync(id);


            private static ServiceResponse CheckResult(IdentityResult result)
            {
                if (result.Succeeded) return new ServiceResponse(true, null);

                var errors = result.Errors.Select(_ => _.Description);
                return new ServiceResponse(false, string.Join(System.Environment.NewLine, errors));

            }
            public   async Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync()
            {
                var UserList = new List<GetUserWithClaimResponseDTO>();
                var allUsers = await userManager.Users.ToListAsync();
                if (allUsers.Count == 0) return UserList;
                foreach (var user in allUsers)
                {
                    var currentUser = await userManager.FindByIdAsync(user.Id);
                    var getCurrentUserClaims = await userManager.GetClaimsAsync(currentUser);
                    if (getCurrentUserClaims.Any())
                        UserList.Add(new GetUserWithClaimResponseDTO()
                        {
                            UserId = user.Id,
                            Email = getCurrentUserClaims.FirstOrDefault(_ => _.Type == ClaimTypes.Email).Value,
                            RoleName = getCurrentUserClaims.FirstOrDefault(_ => _.Type == ClaimTypes.Role).Value,
                            ManagerUser = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(_ => _.Type == "ManageUser").Value),
                            Create = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(_ => _.Type == "Create").Value),
                            Update = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(_ => _.Type == "Update").Value),
                            Delete = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(_ => _.Type == "Delete").Value),
                            Read = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(_ => _.Type == "Read").Value),

                        });
                }

                return UserList;
            }
            public async Task SetUpAsync() => await CreateUserAsync(new CreateUserRequestDTO()
            {
                Name = "Administrateur",
                Email = "admin@admin.com",
                Password = "Admin@123",
                Policy = Policy.AdminPolicy

            });
            public  async Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDTO model)
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user == null) return new ServiceResponse(false, "User not found");
                var oldUserclaims = await userManager.GetClaimsAsync(user);
                Claim[] newUserClaims = [
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, model.RoleName),
                    new Claim("Name", model.Name),
                    new Claim("Create", model.Create.ToString()),
                    new Claim("Update", model.Update.ToString()),
                    new Claim("Read", model.Read.ToString()),
                    new Claim("ManageUser", model.ManagerUser.ToString()),
                    new Claim("Delete", model.Delete.ToString()),
                ];
                var result = await userManager.RemoveClaimsAsync(user, oldUserclaims);
                var response = CheckResult(result);
                if (!response.Flag)
                    return new ServiceResponse(false, response.Message);


                var addNewClaims = await userManager.AddClaimsAsync(user, newUserClaims);
                var outcome = CheckResult(addNewClaims);
                if (outcome.Flag)
                    return new ServiceResponse(true, "User updated");
                else
                    return outcome;
            }
             public  async Task SaveActivityAsync(ActivityTrackerRequestDTO model)
             {
             context.ActivityTracker.Add(model.Adapt(new Tracker()));
             await context.SaveChangesAsync();
             }
             public async Task<IEnumerable<ActivityTrackerResponseDTO>> GetActivitiesAsync()
             {
             var list = new List<ActivityTrackerResponseDTO>();
            var data = (await context.ActivityTracker.ToListAsync()).Adapt<List<ActivityTrackerResponseDTO>>();
             foreach (var activity in data)
             {
            activity.UserName=(await FindUserById(activity.UserId)).Name;
               list.Add(activity);



             }
             return data;
            }

        public Task<IEnumerable<GetUserWithClaimResponseDTO>> getUserwithClaims()
        {
            throw new NotImplementedException();
        }
    }
    } 

    

