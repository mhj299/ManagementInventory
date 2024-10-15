using Application.DTO.Request.Identity;
using Application.DTO.Response;
using Application.DTO.Response.ActivityTracker;
using Application.DTO.Request.ActivityTracker;
using Application.DTO.Response.Identity;
using Application.Interface.Identity;

namespace Application.Service
{
    public class AccountService(IAccountService account) : IAccountService

    {
        public async Task<ServiceResponse> CreateUserAsync(CreateUserRequestDTO model)

           => await account.CreateUserAsync(model);


        public async Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync()

             => await account.GetUsersWithClaimsAsync();
        public async Task<ServiceResponse> LoginAsync(LoginUserRequestDTO model)
        => await account.LoginAsync(model);

        public async Task SetUpAsync() => await account.SetUpAsync();


        public Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDTO model)
        => account.UpdateUserAsync(model);

        private async Task<IEnumerable<ActivityTrackerResponseDTO>> GetActivitiesAsync()
         => await account.GetActivitiesAsync();
        public Task SaveActivityAsync(ActivityTrackerRequestDTO model)
         => account.SaveActivityAsync(model);

         public async Task<IEnumerable<IGrouping<DateTime, ActivityTrackerResponseDTO>>> GroupActivitiesByDateAsync()
         {
         var data = (await GetActivitiesAsync()).GroupBy(e => e.Date).AsEnumerable();
            return data;

        }

        Task<IEnumerable<ActivityTrackerResponseDTO>> IAccountService.GetActivitiesAsync()
        {
            return account.GetActivitiesAsync();
        }
    }
}
