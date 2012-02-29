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

        /// <summary>
        /// Split a string given an arbitrary collection of separators.
        /// </summary>
        /// <param name="value">String to be split.</param>
        /// <param name="delimiter">Collection of zero or more separators.  If null or empty string is provided, comma is used as a default.</param>
        /// <returns>List of String values.</returns>
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
            if (String.IsNullOrEmpty(clrString))
            {
                // Return an empty collection
                return new string[] { };
            }
            char[] clrDelimiter = delimiter.ToString().ToCharArray();
            return clrString.Split(clrDelimiter);
        }

        /// <summary>
        /// Split a comma separated string.
        /// </summary>
        /// <param name="value">String to be split.</param>
        /// <returns>List of String values.</returns>
        [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.None, FillRowMethodName = "FillStringRow", TableDefinition = "item nvarchar(max)")]
        public static IEnumerable SplitString(SqlString value)
        {
            return SplitDelimitedString(value, new SqlString(","));
        }

        /// <summary>
        /// Split a string representing a comma separated list of numbers into the constituent numbers.
        /// Values that cannot be converted to an integer are not returned.
        /// </summary>
        /// <param name="value">String to be split.</param>
        /// <returns>List of integer values.</returns>
        [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.None, FillRowMethodName = "FillNumberRow", TableDefinition = "item int")]
        public static IEnumerable SplitStringIntoNumbers(SqlString value)
        {
            if (value.IsNull)
            {
                yield break; // Returns an empty set
            }

            string clrString = value.ToString();
            if (String.IsNullOrEmpty(clrString))
            {
                yield break; // Returns an empty set
            }

            foreach (string s in clrString.Split(DefaultDelimiter))
            {
                int number;
                if (Int32.TryParse(s, out number))
                {
                    yield return number;
                }
            }            
        }

        /// <summary>
        /// FillNumberRow is here as a consequence of how TVP UDFs have to be programmed.
        /// </summary>
        /// <param name="obj">Numeric value</param>
        /// <param name="item">Numeric value converted to SqlInt32</param>
        public static void FillNumberRow(Object obj, out SqlInt32 item)
        {
            item = new SqlInt32((Int32)obj);
        }

        /// <summary>
        /// FillStringRow is here as a consequence of how TVP UDFs have to be programmed.
        /// </summary>
        /// <param name="obj">String value</param>
        /// <param name="item">String value converted to a SqlString</param>
        public static void FillStringRow(Object obj, out SqlString item)
        {
            item = new SqlString((String)obj);
        }
    }
}

