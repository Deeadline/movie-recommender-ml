﻿using Recommend_Movie_System.Helpers;
using Recommend_Movie_System.Models.Entity;
using Recommend_Movie_System.Models.Request;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services.Interface;
using System;
using System.Linq;
using Recommend_Movie_System.Models.Response;

namespace Recommend_Movie_System.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;

        public UserService(ApplicationContext context)
        {
            _context = context;
        }

        public bool Register(RegistrationRequest request)
        {
            if (_context.users.FirstOrDefault(item => item.email == request.email) != null)
                throw new Exception("Username \"" + request.email + "\" is already taken");

            Encryption.CreatePasswordHash(request.password, out var passwordHash, out var passwordSalt);
            var user = new User
            {
                email = request.email,
                firstName = request.firstName,
                lastName = request.lastName,
                passwordSalt = passwordSalt,
                passwordHash = passwordHash,
                role = request.role
            };

            _context.users.Add(user);

            _context.SaveChanges();

            return true;
        }

        public UserResponse Get(int id)
        {
            var user = _context.users.FirstOrDefault(item => item.id == id);
            return new UserResponse
            {
                id = user.id,
                email = user.email,
                firstName = user.firstName,
                lastName = user.lastName,
                role = user.role.ToLower()
            };
        }
    }
}