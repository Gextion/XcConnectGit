//-----------------------------------------------------------------------
// <copyright company="James Skimming">
//     Copyright (c) 2013 James Skimming
// </copyright>
//-----------------------------------------------------------------------

namespace Gextion.CRM.Models.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// Minimal class for a <see cref="MultitenantIdentityUserLogin{TKey, TTenant}"/> with a <see cref="string"/> user
    /// <see cref="IdentityUserLogin{TKey}.UserId"/>
    /// and <see cref="MultitenantIdentityUserLogin{TKey, TTenant}.CompanyId"/>.
    /// </summary>
    public class MultitenantIdentityUserLogin : MultitenantIdentityUserLogin<string, string>
    {
    }
}
