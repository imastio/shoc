using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Mailing.Model;
using Shoc.Settings.Model;
using Shoc.Settings.Services;

namespace Shoc.Settings.Controllers;

/// <summary>
/// The mailing profiles
/// </summary>
[Route("api/mailing-profiles")]
[ApiController]
[ShocExceptionHandler]
public class MailingProfileController : ControllerBase
{
    /// <summary>
    /// The mailing profile service
    /// </summary>
    private readonly MailingProfileService mailingProfileService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="mailingProfileService">The mailing profile service</param>
    public MailingProfileController(MailingProfileService mailingProfileService)
    {
        this.mailingProfileService = mailingProfileService;
    }

    /// <summary>
    /// Gets all the profiles
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(SettingsAccesses.SETTINGS_MAILING_PROFILES_LIST)]
    [HttpGet]
    public Task<IEnumerable<MailingProfile>> GetAll()
    {
        return this.mailingProfileService.GetAll();
    }

    /// <summary>
    /// Gets the profile by id
    /// </summary>
    /// <param name="id">The id of the profile</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SettingsAccesses.SETTINGS_MAILING_PROFILES_READ)]
    [HttpGet("{id}")]
    public Task<MailingProfile> GetById(string id)
    {
        return this.mailingProfileService.GetById(id);
    }

    /// <summary>
    /// Creates a profile object 
    /// </summary>
    /// <param name="input">Creates the profile based on the input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SettingsAccesses.SETTINGS_MAILING_PROFILES_CREATE)]
    [HttpPost]
    public Task<MailingProfile> Create([FromBody] CreateMailingProfileInput input)
    {
        return this.mailingProfileService.Create(input);
    }

    /// <summary>
    /// Updates the profile password object 
    /// </summary>
    /// <param name="id">The id of the profile</param>
    /// <param name="input">Updates the profile password based on input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SettingsAccesses.SETTINGS_MAILING_PROFILES_EDIT)]
    [HttpPut("{id}/password")]
    public Task<MailingProfile> UpdatePassword(string id, [FromBody] UpdateProfilePasswordInput input)
    {
        return this.mailingProfileService.UpdatePassword(id, input);
    }

    /// <summary>
    /// Updates the profile api secret object 
    /// </summary>
    /// <param name="id">The id of the profile</param>
    /// <param name="input">Updates the profile api secret based on input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SettingsAccesses.SETTINGS_MAILING_PROFILES_EDIT)]
    [HttpPut("{id}/api-secret")]
    public Task<MailingProfile> UpdateApiSecret(string id, [FromBody] UpdateProfileApiSecretInput input)
    {
        return this.mailingProfileService.UpdateApiSecret(id, input);
    }

    /// <summary>
    /// Deletes the profile by id
    /// </summary>
    /// <param name="id">The id of the profile</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(SettingsAccesses.SETTINGS_MAILING_PROFILES_DELETE)]
    [HttpDelete("{id}")]
    public Task<MailingProfile> DeleteById(string id)
    {
        return this.mailingProfileService.DeleteById(id);
    }
}