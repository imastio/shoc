using System;

namespace Shoc.Identity.Model.User;

/// <summary>
/// The user profile model
/// </summary>
public class UserProfileModel
{
    /// <summary>
    /// The user id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The phone number
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// If phone is verified
    /// </summary>
    public bool PhoneVerified { get; set; }

    /// <summary>
    /// The first name
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// The last name
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// The gender 
    /// </summary>
    public string Gender { get; set; }

    /// <summary>
    /// The birth date
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// The user's country
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// The state of user
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// The city of user
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// The postal code of user
    /// </summary>
    public string Postal { get; set; }

    /// <summary>
    /// The first address
    /// </summary>
    public string Address1 { get; set; }

    /// <summary>
    /// The second address
    /// </summary>
    public string Address2 { get; set; }
}