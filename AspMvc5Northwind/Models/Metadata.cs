/**
 * This file, along with PartialClasses.cs, provides metadata for 
 * EF6-generated database objects. This method, using partial classes, 
 * allows for user-defined data annotations to be made on generated 
 * source code. Such annotations can be used for enhanced front-end data validation.
 * 
 * Since this file is not part of the EF6-generated code, the annotations 
 * remain if the generated code needs to be re-generated, for example
 * if a column is added to a database table.
 * 
 * For more info see
 * http://www.asp.net/mvc/tutorials/mvc-5/database-first-development/enhancing-data-validation
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspMvc5Northwind.Models
{
    public partial class ProductMetadata
    {
        [StringLength(40)]
        public string ProductName;
    }
}