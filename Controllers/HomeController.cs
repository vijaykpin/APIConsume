using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using APIConsume.Models;



using Newtonsoft.Json;
using System.Text;

namespace APIConsume.Controllers
{
	public class HomeController: Controller
	{

        public async Task<IActionResult> Index()
        {
            List<Employee> employeeList = new List<Employee>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://webapiemp.azurewebsites.net/api/Employee/GetEmployee"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    employeeList = JsonConvert.DeserializeObject<List<Employee>>(apiResponse);
                }
            }
            return View(employeeList);
        }

        public ViewResult GetEmployee() => View();

        [HttpPost]
        public async Task<IActionResult> GetEmployee(int id)
        {
            Employee employee = new Employee();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://webapiemp.azurewebsites.net/api/Employee/GetEmployeeId?id=" + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        employee = JsonConvert.DeserializeObject<Employee>(apiResponse);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return View(employee);
        }



        public ViewResult AddEmployee() => View();

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            Employee receivedEmployee = new Employee();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://webapiemp.azurewebsites.net/api/Employee/AddEmployee", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedEmployee = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }
            return View(receivedEmployee);
        }


        public async Task<IActionResult> UpdateEmployee(int id)
        {
            Employee employee = new Employee();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://webapiemp.azurewebsites.net/api/Employee/GetEmployeeId?id=" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            Employee receivedEmployee = new Employee();
            using (var httpClient = new HttpClient())
            {
                // var content = new MultipartFormDataContent();
                var content = new MultipartFormDataContent
              
                content.Add(new StringContent(employee.Id.ToString()), "Id");
                content.Add(new StringContent(employee.Name), "Name");
                content.Add(new StringContent(employee.Department), "Department");
                content.Add(new StringContent(employee.Salary.ToString()), "Salary");

                StringContent content1 = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://webapiemp.azurewebsites.net/api/Employee/EditEmployee", content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receivedEmployee = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }
            return View(receivedEmployee);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int EmployeeId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://webapiemp.azurewebsites.net/api/Employee/DeleteEmployee?id=" + EmployeeId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }

        
    }
}
