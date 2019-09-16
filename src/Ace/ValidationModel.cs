using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ace
{
    public abstract class ValidationModel
    {
        /// <summary>
        /// 数据验证
        /// </summary>
        public virtual void Validate()
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext vc = new ValidationContext(this, null, null);
            bool isValid = Validator.TryValidateObject
                    (this, vc, validationResults, true);
            if (isValid == false)
            {
                throw new InvalidInputException(validationResults[0].ErrorMessage);
            }
        }
    }

}
