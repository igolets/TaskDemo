using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using TaskDemo.Web.Models;

namespace TaskDemo.Web.Tests.Models
{
    [TestClass]
    public class TaskCreateModelUnitTest
    {
        [TestMethod]
        public void Validate_NameEmpty()
        {
            // Arrange
            var model = new TaskCreateModel
            {
                Name = ""
            };
            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            var valid = Validator.TryValidateObject(model, context, result, true);

            // Assert
            valid.ShouldBe(false);
        }

        [TestMethod]
        public void Validate_NameLong()
        {
            // Arrange
            var model = new TaskCreateModel
            {
                Name = "Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Very Long Text"
            };
            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            var valid = Validator.TryValidateObject(model, context, result, true);

            // Assert
            valid.ShouldBe(false);
        }

        [TestMethod]
        public void Validate_Success()
        {
            // Arrange
            var model = new TaskCreateModel
            {
                Name = "OK"
            };
            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            // Act
            var valid = Validator.TryValidateObject(model, context, result, true);

            // Assert
            valid.ShouldBe(true);
        }
    }
}
