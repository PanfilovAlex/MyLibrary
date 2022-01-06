using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using WebApiMyLib.Data.Repositories;


namespace WebApiMyLib.BLL.Services
{
    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _authorRepository;
        private IValidationService<Author> _validationService;

        public AuthorService(IValidationService<Author> validationService, IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
            _validationService = validationService;
        }
        public IEnumerable<Author> Authors(BookPageParameters pageParameters)
            => _authorRepository.Authors(pageParameters);

        public Author Add(Author author)
        {
            var validationResult = _validationService.Validate(author);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            try
            {
                _authorRepository.Add(author);
            }
            catch
            {
                return null;
            }

            return _authorRepository.GetAuthors.FirstOrDefault(a => a.FirstName.Equals(author.FirstName) 
            && a.LastName.Equals(author.LastName));
        }

        public void Delete(int id)
        {
            var authorToDelete = _authorRepository.GetAuthors.FirstOrDefault(author => author.Id == id);
            if (authorToDelete == null)
            {
                return;
            }
            _authorRepository.Delete(id);
        }

        public Author Find(int id)
        {
            var foundAuthor = _authorRepository.Find(id);
            if (foundAuthor == null)
            {
                return null;
            }
            return foundAuthor;
        }

        public Author Update(Author author)
        {
            var validationResult = _validationService.Validate(author);
            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            try
            {
                _authorRepository.Update(author);
            }
            catch
            {
                throw new Exception("Author wasn't updated.");
            }

            return _authorRepository.GetAuthors.FirstOrDefault(a => a.FirstName.Equals(author.FirstName)
            && a.LastName.Equals(author.LastName));
        }
    }
}
