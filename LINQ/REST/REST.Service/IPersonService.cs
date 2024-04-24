using REST.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST.Service
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetPeople(List<Person> people);
        Task<IEnumerable<Person>> GetAdults(List<Person> people);
        Task<IQueryable<Person>> PeoplePagination(List<Person> people, int page, int size);
        Task<Person> GetPersonById(List<Person> people, int id);
        Task<IEnumerable<Person>> SortByAge(List<Person> people);
        Task<IEnumerable<Person>> SortByAgeAndName(List<Person> people);      
        Task<IEnumerable<IGrouping<string, Person>>> GroupByCity(List<Person> people);
        Task<string> AvgAge(List<Person> people);
        Task<string> MinMaxAge(List<Person> people);
    }
}
