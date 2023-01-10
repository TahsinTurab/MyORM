# MyORM
This is a simple ORM Framework for CRUD Operation in C#

This ORM can done the following operation:
            1. Insert(T item)
            2. Update(T item)
            3. Delete (T item)
            4. Delete (G id)
            5. GetById(G id)
            6. GetAll()
Here, T should be a class that always has an id of type G. You can make an abstract base class or interface to enforce this in generic constraint

This ORM able to read and write in a database table. Database tables have to be presented from before.
Useed Reflection and Ado.net to create this mini ORM system.

This ORM can handle nested objects. List of objects and primitive types as well. Nesting will happen on multiple levels so recursion is used. 

Nested delete operation should not use the Cascade feature, delete recursively. 
