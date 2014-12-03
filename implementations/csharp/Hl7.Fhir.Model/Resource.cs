﻿/*
  Copyright (c) 2011-2012, HL7, Inc
  All rights reserved.
  
  Redistribution and use in source and binary forms, with or without modification, 
  are permitted provided that the following conditions are met:
  
   * Redistributions of source code must retain the above copyright notice, this 
     list of conditions and the following disclaimer.
   * Redistributions in binary form must reproduce the above copyright notice, 
     this list of conditions and the following disclaimer in the documentation 
     and/or other materials provided with the distribution.
   * Neither the name of HL7 nor the names of its contributors may be used to 
     endorse or promote products derived from this software without specific 
     prior written permission.
  
  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
  IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
  
*/



using Hl7.Fhir.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Model
{
    [InvokeIValidatableObject]
    public abstract partial class Resource 
    {
        public static string GetResourceTypeName(Resource resource)
        {
            if (resource == null) throw Error.ArgumentNull("resource");

            return GetResourceTypeName(resource.GetType());
        }

        public static string GetResourceTypeName(Type type)
        {
            return type.Name;       // trivial now, but might be a map
        }


       // public string ResourceName { get { return GetResourceTypeName(this); } }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (Id != null && !new Uri(Id,UriKind.RelativeOrAbsolute).IsAbsoluteUri)
                result.Add(DotNetAttributeValidation.BuildResult(validationContext, "Entry id must be an absolute URI"));

            if(Meta != null)
            {
                if (!String.IsNullOrEmpty(this.Meta.VersionId) && !new Uri(Id,UriKind.RelativeOrAbsolute).IsAbsoluteUri)
                    result.Add(DotNetAttributeValidation.BuildResult(validationContext, "Entry selflink must be an absolute URI"));

                if (Meta.Tag != null && validationContext.ValidateRecursively())
                    DotNetAttributeValidation.TryValidate(Meta.Tag,result,true);
            }

            return result;
        }
    }
}


