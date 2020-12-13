using Newtonsoft.Json;
using RockContent.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockContent.Features.ArticleLike.Command.Validator
{
    public interface IRequestValidator
    {
        bool isValidRequest { get; }

        string errorMessage { get; }

        RequestValidator SetRequestValidator(LikeDbModel likeDbModel);

        RequestValidator ValidateAnyNull();
    }

    public class RequestValidator : IRequestValidator
    {
        private LikeDbModel LikeDbModel;
        public bool isValidRequest { get; private set; }

        public string errorMessage { get; private set; }

        public RequestValidator SetRequestValidator(LikeDbModel likeDbModel)
        {
            this.isValidRequest = true;
            this.LikeDbModel = likeDbModel;
            return this;
        }

        public RequestValidator ValidateAnyNull()
        {
            var dictionary = LikeDbModel.GetType()
                                         .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                                         .ToDictionary(a => a.Name, a => (object)a.GetValue(LikeDbModel, null));

            if (dictionary.Any(a => a.Value == null))
            {
                isValidRequest = false;
                throw new ArgumentNullException("Request Contains Invalid Arguments");
            }

            return this;

        }
    }
}
