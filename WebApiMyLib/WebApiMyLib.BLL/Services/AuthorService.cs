using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;
using WebApiMyLib.Data.Repositories;


namespace WebApiMyLib.BLL.Services
{
    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _authorRepository;
        private IValidationService<Author> _validationService;

        public AuthorService(IAuthorRepository authorRepository,
            IValidationService<Author> validationService)
        {
            _authorRepository = authorRepository;
            _validationService = validationService;
        }

        public IEnumerable<Author> GetAuthors => _authorRepository.GetAuthors;
        public IEnumerable<Author> Authors(BookPageParameters pageParameters)
            => _authorRepository.Authors(pageParameters, author => author.IsDeleted == false);

        public Author Add(Author author)
        {
            var validationResult = _validationService.Validate(author);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            try
            {
                return _authorRepository.Add(author);
            }
            catch(Exception ex)
            {
                throw;
            }
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
                throw new Exception("Author was not added");
            }

            return foundAuthor;
        }

        public Author Update(Author author)
        {
            var updatedAuthor = new Author();
            var validationResult = _validationService.Validate(author);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            try
            {
                updatedAuthor = _authorRepository.Update(author);
            }
            catch
            {
                throw new Exception("Author wasn't updated.");
            }

            return updatedAuthor;
        }
    }
}
