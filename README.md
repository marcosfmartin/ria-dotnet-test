# This repository concerns the coding test for Ria Transfer

1) ATM Cartridge problem
   This solution is in a Console App project called ATMCartridge. You can just run and it will print all possible solutions for the inputs in the test.
   
   My idea here was to divide the amounts by each of the possible cartridge. I get the maximum amount possible for that note (e.g. 230/100 = 2, so I can have at most 2 100 cartridges).
   This leaves me with a pretty small array for each cartridge to work with. After that, I combine the possibilities for each notes, making some optimizations to allow me to run faster.
   Even though I'm not a fan of comments in the code, I needed to put some to make it clearer what I was doing in each method/region

3) REST Server
   1) To test this, simply run the RiaServer project. This will open the swagger UI in the browser. You will have a get and a post endpoint. The get is responsible for fetching the current array. The post will insert in the array and update a SQLite cache in order to persist the changes (library used for the cache: https://github.com/neosmart/SqliteCache)
   2) The simulator is in the CustomerTest project, which is a simple console app. To run it, simply click select the CustomerTestProject and run it after you have ran the RiaServer project. You can the run the get endpoint to see the updated customer list after the paralell requests run
