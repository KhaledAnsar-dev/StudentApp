using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Student_client_wf.Models;
using System.Net.Http.Json;

namespace Student_client_wf.Services
{
    public class StudentService
    {
        static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7249/api/students/")
        };

        public StudentService()
        {
        }

        public async Task<List<StudentDTO>> GetAllStudentsAsync()
        {
            try
            {
                var students = await _httpClient.GetFromJsonAsync<List<StudentDTO>>("All");

                return students != null ? students : null;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching students", ex);
            }
        }
        public async Task<StudentDTO?> AddStudentAsync(StudentDTO newStudent)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("", newStudent);

                if (response.IsSuccessStatusCode)
                {
                    var addedStudent = await response.Content.ReadFromJsonAsync<StudentDTO>();

                    return addedStudent;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding student", ex);
            }
        }
        public async Task<bool> UpdateStudentAsync(int studentID, StudentDTO newStudent)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{studentID}", newStudent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating student", ex);
            }
        }
        public async Task<bool> DeleteStudentAsync(int studentID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{studentID}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting student", ex);
            }
        }
        public static async Task<StudentDTO?> FindAsync(int studentID)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Find/{studentID}");

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<StudentDTO>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving student with ID [{studentID}]", ex);
            }
        }

    }
}
