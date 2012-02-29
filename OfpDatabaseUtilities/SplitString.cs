// <copyright file="SplitString.cs" company="Secretariat of the Pacific Community">
//  Copyright 2012 Secretariat of the Pacific Community
// </copyright>
[assembly: System.CLSCompliant(true)]
namespace Spc.Ofp
{
    /*
     * This file is part of TUBS.
     *
     * TUBS is free software: you can redistribute it and/or modify
     * it under the terms of the GNU Affero General Public License as published by
     * the Free Software Foundation, either version 3 of the License, or
     * (at your option) any later version.
     *  
     * TUBS is distributed in the hope that it will be useful,
     * but WITHOUT ANY WARRANTY; without even the implied warranty of
     * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     * GNU Affero General Public License for more details.
     *  
     * You should have received a copy of the GNU Affero General Public License
     * along with TUBS.  If not, see <http://www.gnu.org/licenses/>.
     */
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using Microsoft.SqlServer.Server;
    using System.Collections;

    /// <summary>
    /// This partial class holds String related functions.
    /// </summary>
    public partial class UserDefinedFunctions
    {
        private static readonly char[] DefaultDelimiter = { ',' };

        [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.None, FillRowMethodName = "FillStringRow", TableDefinition = "item nvarchar(max)")]
        public static IEnumerable SplitDelimitedString(SqlString value, SqlString delimiter)
        {
            if (value.IsNull)
            {
                // Return an empty collection
                return new string[] { };
            }

            // Assume this is what the user wants
            if (delimiter.IsNull)
            {
                delimiter = new SqlString(",");
            }

            string clrString = value.ToString();
            char[] clrDelimiter = delimiter.ToString().ToCharArray();
            return clrString.Split(clrDelimiter);
        }


        [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.None, FillRowMethodName = "FillStringRow", TableDefinition = "item nvarchar(max)")]
        public static IEnumerable SplitString(SqlString value)
        {
            return SplitDelimitedString(value, new SqlString(","));
        }

        [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.None, FillRowMethodName = "FillNumberRow", TableDefinition = "item int")]
        public static IEnumerable SplitStringIntoNumbers(SqlString value)
        {
            if (value.IsNull)
            {
                yield break;
            }

            foreach (string s in value.ToString().Split(DefaultDelimiter))
            {
                int number;
                if (Int32.TryParse(s, out number))
                {
                    yield return number;
                }
            }            
        }

        public static void FillNumberRow(Object obj, out SqlInt32 item)
        {
            item = new SqlInt32((Int32)obj);
        }

        public static void FillStringRow(Object obj, out SqlString item)
        {
            item = new SqlString((String)obj);
        }
    }
}

