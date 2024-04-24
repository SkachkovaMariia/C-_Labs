using REST.Domain;

namespace REST.Service
{
    public class PersonService : IPersonService
    {
        public async Task<IEnumerable<Person>> GetPeople(List<Person> people)
        {
            return await Task.Run(() =>
            {
                return people.AsParallel().ToList();
            });
        }

        public async Task<IEnumerable<Person>> GetAdults(List<Person> people)
        {
            return await Task.FromResult(people
                .Where(x => x.Age >= 18));
        }

        public async Task<IQueryable<Person>> PeoplePagination(List<Person> people, int page, int size)
        {
            var pagePeople = people
                .Skip((page - 1) * size)
                .Take(size)
                .AsQueryable();

            return await Task.FromResult(pagePeople);
        }

        public async Task<Person> GetPersonById(List<Person> people, int id)
        {
            return await Task.FromResult(people
                .SingleOrDefault(x => x.Id == id));
        }

        public async Task<IEnumerable<Person>> SortByAge(List<Person> people)
        {
            return await Task.FromResult(people
                .OrderByDescending(x => x.Age));
        }

        public async Task<IEnumerable<Person>> SortByAgeAndName(List<Person> people)
        {
            return await Task.FromResult(people
                .OrderByDescending(x => x.Age)
                .ThenBy(x => x.Name));
        }       

        public async Task<IEnumerable<IGrouping<string, Person>>> GroupByCity(List<Person> people)
        {
            return await Task.FromResult(people
                .GroupBy(x => x.City));
        }

        public async Task<string> AvgAge(List<Person> people)
        {
            if (people.Any())
            {
                return await Task.FromResult($"Average person age - {Math.Round((double)people
                .Sum(x => x.Age) / people.Count(), 2)}");
            }

            return "List is empty";
        }

        public async Task<string> MinMaxAge(List<Person> people)
        {
            if (people.Any())
            {
                return await Task.FromResult($"Min age - {people.Min(x => x.Age)}" +
                    $"\nMax age - {people.Max(x => x.Age)}");
            }

            return "List is empty";
        }
    }
}
