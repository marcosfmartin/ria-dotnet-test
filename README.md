# This repository concerns the coding test for Ria Transfer

1) ATM Cartridge problem

2) REST Server
   1) To test this, simply run the RiaServer project. This will open the swagger UI in the browser. You will have a get and a post endpoint. The get is responsible for fetching the current array. The post will insert in the array and update a SQLite cache in order to persist the changes (library used for the cache: https://github.com/neosmart/SqliteCache)
   2) The simulator is in the CustomerTest project, which is a simple console app. To run it, simply click select the CustomerTestProject and run it after you have ran the RiaServer project. You can the run the get endpoint to see the updated customer list.
