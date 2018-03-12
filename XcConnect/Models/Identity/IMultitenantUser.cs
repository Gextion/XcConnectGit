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
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Minimal interface for a <see cref="IMultitenantUser{TKey, TTenant}"/> with a <see cref="string"/> user
    /// <see cref="IUser{TKey}.Id"/> and <see cref="IMultitenantUser{TKey,TTenant}.CompanyId"/>.
    /// </summary>
    public interface IMultitenantUser : IMultitenantUser<string, string>
    {
    }
}
