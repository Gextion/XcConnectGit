//-----------------------------------------------------------------------
// <copyright company="James Skimming">
//     Copyright (c) 2013 James Skimming
// </copyright>
//-----------------------------------------------------------------------

namespace Gextion.CRM.Models.Identity
{
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Interface defining a multi-tenant user.
    /// </summary>
    /// <typeparam name="TKey">The type of <see cref="IUser{TKey}.Id"/> for a user.</typeparam>
    /// <typeparam name="TTenantKey">The type of <see cref="CompanyId"/> for a user.</typeparam>
    public interface IMultitenantUser<out TKey, TTenantKey> : IUser<TKey>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        TTenantKey CompanyId { get; set; } 
    }
}
