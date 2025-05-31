using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Launcher;
using SPTarkov.Server.Core.Models.Eft.Profile;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Launcher;
using SPTarkov.Server.Core.Utils;

namespace SPTarkov.Server.Core.Callbacks;

[Injectable]
public class ProfileCallbacks(
    HttpResponseUtil _httpResponse,
    TimeUtil _timeUtil,
    ProfileController _profileController,
    ProfileHelper _profileHelper
)
{
    /// <summary>
    ///     Handle client/game/profile/create
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> CreateProfile(string url, ProfileCreateRequestData info, string sessionID)
    {
        var id = _profileController.CreateProfile(info, sessionID);
        return new ValueTask<string>(_httpResponse.GetBody(
            new CreateProfileResponse
            {
                UserId = id
            }
        ));
    }

    /// <summary>
    ///     Handle client/game/profile/list
    ///     Get the complete player profile (scav + pmc character)
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> GetProfileData(string url, EmptyRequestData _, string sessionID)
    {
        return new ValueTask<string>(_httpResponse.GetBody(_profileController.GetCompleteProfile(sessionID)));
    }

    /// <summary>
    ///     Handle client/game/profile/savage/regenerate
    ///     Handle the creation of a scav profile for player
    ///     Occurs post-raid and when profile first created immediately after character details are confirmed by player
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> RegenerateScav(string url, EmptyRequestData _, string sessionID)
    {
        return new ValueTask<string>(_httpResponse.GetBody(
            new List<PmcData>
            {
                _profileController.GeneratePlayerScav(sessionID)
            }
        ));
    }

    /// <summary>
    ///     Handle client/game/profile/voice/change event
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> ChangeVoice(string url, ProfileChangeVoiceRequestData info, string sessionID)
    {
        _profileController.ChangeVoice(info, sessionID);
        return new ValueTask<string>(_httpResponse.NullResponse());
    }

    /// <summary>
    ///     Handle client/game/profile/nickname/change event
    ///     Client allows player to adjust their profile name
    /// </summary>
    /// <returns>Client response as string</returns>
    public ValueTask<string> ChangeNickname(string url, ProfileChangeNicknameRequestData info, string sessionId)
    {
        var output = _profileController.ChangeNickname(info, sessionId);

        return output switch
        {
            NicknameValidationResult.Taken => new ValueTask<string>(_httpResponse.GetBody<object?>(null, BackendErrorCodes.NicknameNotUnique, $"{BackendErrorCodes.NicknameNotUnique} - ")),
            NicknameValidationResult.Short => new ValueTask<string>(_httpResponse.GetBody<object?>(null, BackendErrorCodes.NicknameNotValid, $"{BackendErrorCodes.NicknameNotValid} - ")),
            _ => new ValueTask<string>(_httpResponse.GetBody<object>(
                new
                {
                    status = 0,
                    NicknameChangeDate = _timeUtil.GetTimeStamp()
                }
            ))
        };
    }

    /// <summary>
    ///     Handle client/game/profile/nickname/validate
    /// </summary>
    /// <returns>Client response as string</returns>
    public ValueTask<string> ValidateNickname(string url, ValidateNicknameRequestData info, string sessionId)
    {
        return _profileController.ValidateNickname(info, sessionId) switch
        {
            NicknameValidationResult.Taken => new ValueTask<string>(_httpResponse.GetBody<object?>(null, BackendErrorCodes.NicknameNotUnique, $"{BackendErrorCodes.NicknameNotUnique} - ")),
            NicknameValidationResult.Short => new ValueTask<string>(_httpResponse.GetBody<object?>(null, BackendErrorCodes.NicknameNotValid, $"{BackendErrorCodes.NicknameNotValid} - ")),
            _ => new ValueTask<string>(_httpResponse.GetBody(
                new
                {
                    status = "ok"
                }
            ))
        };
    }

    /// <summary>
    ///     Handle client/game/profile/nickname/reserved
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> GetReservedNickname(string url, EmptyRequestData _, string sessionId)
    {
        var fullProfile = _profileHelper.GetFullProfile(sessionId);
        if (fullProfile?.ProfileInfo?.Username is not null)
        {
            // Send players name back to them
            return new ValueTask<string>(_httpResponse.GetBody(fullProfile?.ProfileInfo?.Username));
        }

        return new ValueTask<string>(_httpResponse.GetBody("SPTarkov"));
    }

    /// <summary>
    ///     Handle client/profile/status
    ///     Called when creating a character when choosing a character face/voice
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> GetProfileStatus(string url, EmptyRequestData _, string sessionId)
    {
        return new ValueTask<string>(_httpResponse.GetBody(_profileController.GetProfileStatus(sessionId)));
    }

    /// <summary>
    ///     Handle client/profile/view
    ///     Called when viewing another players profile
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> GetOtherProfile(string url, GetOtherProfileRequest request, string sessionID)
    {
        return new ValueTask<string>(_httpResponse.GetBody(_profileController.GetOtherProfile(sessionID, request)));
    }

    /// <summary>
    ///     Handle client/profile/settings
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> GetProfileSettings(string url, GetProfileSettingsRequest info, string sessionID)
    {
        return new ValueTask<string>(_httpResponse.GetBody(_profileController.SetChosenProfileIcon(sessionID, info)));
    }

    /// <summary>
    ///     Handle client/game/profile/search
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> SearchProfiles(string url, SearchProfilesRequestData info, string sessionID)
    {
        return new ValueTask<string>(_httpResponse.GetBody(_profileController.SearchProfiles(info, sessionID)));
    }

    /// <summary>
    ///     Handle launcher/profile/info
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> GetMiniProfile(string url, GetMiniProfileRequestData info, string sessionID)
    {
        return new ValueTask<string>(_httpResponse.NoBody(_profileController.GetMiniProfile(sessionID)));
    }

    /// <summary>
    ///     Handle /launcher/profiles
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> GetAllMiniProfiles(string url, EmptyRequestData _, string sessionID)
    {
        return new ValueTask<string>(_httpResponse.NoBody(_profileController.GetMiniProfiles()));
    }
}
