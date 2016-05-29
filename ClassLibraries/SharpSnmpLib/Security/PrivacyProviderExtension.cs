﻿// IPrivacyProvider extension methods.
// Copyright (C) 2010 Lex Li
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 10/10/2010
 * Time: 6:59 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Extension class for <see cref="IPrivacyProvider"/>.
    /// </summary>
    public static class PrivacyProviderExtension
    {
        /// <summary>
        /// Converts to <see cref="Levels"/>.
        /// </summary>
        /// <returns></returns>
        public static Levels ToSecurityLevel(this IPrivacyProvider privacy)
        {
            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }
                
            Levels flags;
            if (privacy.AuthenticationProvider == DefaultAuthenticationProvider.Instance)
            {
                flags = 0;
            }
            else if (privacy is DefaultPrivacyProvider)
            {
                flags = Levels.Authentication;
            }
            else
            {
                flags = Levels.Authentication | Levels.Privacy;
            }

            return flags;
        } 

        internal static ISnmpData GetScopeData(this IPrivacyProvider privacy, Header header, SecurityParameters parameters, ISnmpData rawScopeData)
        {
            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }

            if (header == null)
            {
                throw new ArgumentNullException("header");
            }

            return Levels.Privacy == (header.SecurityLevel & Levels.Privacy)
                       ? privacy.Encrypt(rawScopeData, parameters)
                       : rawScopeData;
        }
    }
}
