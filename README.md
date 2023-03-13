# gzbits.DuckIt
If it walks like a duck and it quacks like a duck, then it must be a duck.

# Overview
Standard practice is to not return your domain entities via your web interfaces (web api, mvc controller, etc.) as well as the inverse, directly accept domain entities as input. This is typically acheived by created explicit objects to act as this intermedary type (i.e. DTOs).  Using DTOs requires explicit logic on _mapping_ your domain fields to your DTO fields. More often than not, these are 1 to 1.  This library aims to make this specific case only require an interface definition to describe what will be serialized into and from the API end point taking advantage of Swagger/OpenAPI meta data.
