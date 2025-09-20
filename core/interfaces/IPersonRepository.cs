using System.Collections.Generic;


using PersonApp.core.models; 

namespace PersonApp.core.interfaces 
{
    public interface IPersonRepository
    {
      
        void Add(Person person);
        void Update(Person person);
        void Delete(int id);
        List<Person> GetAll();

        
      
    }
}
