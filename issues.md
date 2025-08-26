# Issues I found when testing the endpoints

1- POST /api/School: Creating a school causes a possible object cycle

2- Returning the nav props of an object causes problems - let's do the bare minimum and return only the object itself without its nav props, except for the enrollment because we need to understand what's going on, not just ids/fkeys
