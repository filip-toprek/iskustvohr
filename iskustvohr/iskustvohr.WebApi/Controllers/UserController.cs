using AutoMapper;
using iskustvohr.Model;
using iskustvohr.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using iskustvohr.Service.Common;
using iskustvohr.WebApi.Models;

namespace iskustvohr.WebApi.Controllers 
{
    public class UserController : ApiController
    {
        protected IUserService UserService { get; set; }
        protected IMapper Mapper { get; set; }
        public UserController(IUserService userService, IMapper mapper)
        {
            UserService = userService;
            Mapper = mapper;
        }

        [HttpPost]
        [Route("Api/User/Register")]
        // Create new user
        public async Task<HttpResponseMessage> RegisterUserAsync(RegisterUser userToRegister)
        {
            if (userToRegister == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please fill in the register form!");

            if (!userToRegister.Password.Equals(userToRegister.VerifyPassword))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please enter a correct password!");

            User newUser = Mapper.Map<User>(userToRegister);
            if (await UserService.RegisterUserAsync(newUser) != null)
            {
                return Request.CreateResponse(HttpStatusCode.Created, newUser);
            }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to create user!");
        }

        [HttpGet]
        [Route("Api/User/Logout")]
        // Logout user
        public HttpResponseMessage LogoutUser()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut();
            return Request.CreateResponse(HttpStatusCode.OK, "User was signed out!");
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Business, User")]
        [Route("Api/User/")]
        // Update user
        public async Task<HttpResponseMessage> UpdateUserAsync(UpdateUser userToUpdate)
        {
            if (userToUpdate == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please fill in the update form!");

            User user = await UserService.GetUserByIdAsync(Mapper.Map<User>(userToUpdate));
            if (user == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found!");

            switch(await UserService.UpdateUserProfileAsync(Mapper.Map<User>(userToUpdate)))
            {
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "User updated successfully!");
                case 0:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to update user!");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to update user!");
            }   
        }

        [HttpGet]
        [Route("Api/User/")]
        // Logout user
        public async Task<HttpResponseMessage> GetUserByIdAsync(Guid id)
        {
            User user = new User { Id = id };
            User userToReturn = await UserService.GetUserByIdAsync(user);
            if (userToReturn == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found!");
            return Request.CreateResponse(HttpStatusCode.OK, userToReturn);
        }


        [HttpGet]
        [Route("Api/User/Confirm/{verificationId}")]
        // Confim email
        public async Task<HttpResponseMessage> ConfirmEmailAsync()
        {
            string verificationId = RequestContext.RouteData.Values["verificationId"] as string;
            if (string.IsNullOrEmpty(verificationId))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid or expired verification link.");
            }
            User verifyUser = new User
            {
                EmailVerificationId = Guid.Parse(verificationId)
            };
            switch (await UserService.ConfirmEmailAsync(verifyUser))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to verify user");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.Created, "User verified succesfully");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error");
            }
        }

        [HttpPost]
        [Route("Api/User/Reset")]
        // Reset password
        public async Task<HttpResponseMessage> RequestResetPasswordAsync(User user)
        {
            if (user == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please enter your email.");

            switch (await UserService.RequestResetPasswordAsync(user))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Failed to find a user");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.Created, "Please check your email.");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error");
            }
        }


        [HttpPut]
        [Route("Api/User/Reset")]
        // Reset password
        public async Task<HttpResponseMessage> ChangePasswordAsync(UpdatePassword updatePassword)
        {
            if (updatePassword == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please enter your password.");

            switch (await UserService.ChangePasswordAsync(Mapper.Map<User>(updatePassword)))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Expired");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "Password changed.");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error");
            }
        }

    }
}