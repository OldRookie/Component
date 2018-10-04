using Component.Infrastructure.MetaData;
using Component.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Component.Tests.Infrastructure.Test
{
    public class MetadataValidationTest
    {
        [Fact]
        public void ValidateMetadataModelTest() {
            MetadataTypesRegister.RegisterMetadataType(typeof(ClassA), typeof(ClassAMetadata));
            var obj = new ClassA();
            obj.Validate();
        }
    }

    [MetadataType(typeof(ClassAMetadata))]
    public class ClassA {
        
        public string Name { get; set; }
    }

    public class ClassAMetadata
    {
        [Display(Name = "Hello")]
        [Required]
        public string Name { get; set; }
    }
}
