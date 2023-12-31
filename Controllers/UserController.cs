﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using users.Models;
using static users.Dtos;

namespace users.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        Connect connect = new();
        private readonly List<UserDto> users = new();

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> Get()
        {

            try
            {
                connect.connection.Open();

                string sql = "SELECT * FROM users";

                MySqlCommand cmd = new MySqlCommand(sql, connect.connection);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var result = new UserDto(
                        reader.GetGuid("Id"),
                        reader.GetString("Name"),
                        reader.GetString("Email"),
                        reader.GetInt32("Age"),
                        reader.GetDateTime("Created")
                        );

                    users.Add(result);
                }

                connect.connection.Close();

                return StatusCode(200, users);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> Get(Guid id)
        {

            try
            {
                connect.connection.Open();

                string sql = "SELECT * FROM users WHERE Id=@Id";

                MySqlCommand cmd = new MySqlCommand(sql, connect.connection);

                cmd.Parameters.AddWithValue("Id", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var result = new UserDto(
                        reader.GetGuid("Id"),
                        reader.GetString("Name"),
                        reader.GetString("Email"),
                        reader.GetInt32("Age"),
                        reader.GetDateTime("Created")
                        );

                    connect.connection.Close();
                    return StatusCode(200, result);
                }
                else
                {
                    Exception e = new();
                    connect.connection.Close();
                    return StatusCode(404, e.Message);
                }

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult<User> Post(CreateUserDto createUser)
        {
            //DateTime dateTime = DateTime.Now;
            //string Time = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = createUser.Name,
                Email = createUser.Email,
                Age = createUser.Age,
                Created = DateTimeOffset.Now
            };

            try
            {
                connect.connection.Open();

                //string sql = $"INSERT INTO `users`(`Id`, `Name`, `Email`, `Age`, `Created`) VALUES ('{user.Id}','{user.Name}','{user.Email}',{user.Age},'{time}')";

                string sql = $"INSERT INTO `users`(`Id`, `Name`, `Email`, `Age`, `Created`) VALUES (@Id,@Name,@Email,@Age,@Created)";


                MySqlCommand cmd = new MySqlCommand(sql, connect.connection);

                cmd.Parameters.AddWithValue("Id", user.Id);
                cmd.Parameters.AddWithValue("Name", user.Name);
                cmd.Parameters.AddWithValue("Email", user.Email);
                cmd.Parameters.AddWithValue("Age", user.Age);
                cmd.Parameters.AddWithValue("Created", user.Created);

                cmd.ExecuteNonQuery();

                connect.connection.Close();

                return StatusCode(201, user);


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPut("{id}")]
        public ActionResult<User> Put(UpdateUserDto updateUser, Guid id)
        {
            try
            {
                connect.connection.Open();

                string sql = $"UPDATE `users` SET `Name`=@Name,`Email`=@Email,`Age`=@Age WHERE Id = @Id";


                MySqlCommand cmd = new MySqlCommand(sql, connect.connection);


                cmd.Parameters.AddWithValue("Name", updateUser.Name);
                cmd.Parameters.AddWithValue("Email", updateUser.Email);
                cmd.Parameters.AddWithValue("Age", updateUser.Age);
                cmd.Parameters.AddWithValue("Id", id);

                cmd.ExecuteNonQuery();

                connect.connection.Close();

                return StatusCode(204);


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                connect.connection.Open();

                string sql = $"DELETE FROM users WHERE Id = @Id";


                MySqlCommand cmd = new MySqlCommand(sql, connect.connection);

                cmd.Parameters.AddWithValue("Id", id);

                cmd.ExecuteNonQuery();

                connect.connection.Close();

                return StatusCode(200);


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }
    }
}