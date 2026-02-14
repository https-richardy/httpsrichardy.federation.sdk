# IdentityClient

The `IdentityClient` is your primary tool for managing user authentication and identity-related operations within the Identity Provider. It handles everything from authenticating users and creating new identities to invalidating sessions.

This guide provides a straightforward, developer-focused overview of how to use the `IdentityClient`.

## Table of Contents

- [Main Methods](#main-methods)
  - [AuthenticateAsync](#authenticateasync)
  - [CreateIdentityAsync](#createidentityasync)
  - [InvalidateSessionAsync](#invalidatesessionasync)
- [Predictable Error Handling](#predictable-error-handling)

## Main Methods

The client exposes a set of asynchronous methods that return a `Result` or `Result<T>`. This object standardizes the outcome of each operation, telling you whether it succeeded and providing either the requested data or a specific error.

### AuthenticateAsync

Authenticates a user with their credentials and returns a `Result<AuthenticationResult>` containing the access and refresh tokens upon success.

**Example: Authenticating a user**

```csharp
var credentials = new AuthenticationCredentials("user@email.com", "password123");
var result = await identityClient.AuthenticateAsync(credentials);

if (result.IsSuccess)
{
    Console.WriteLine($"Authentication successful! Token: {result.Data.AccessToken}");
}
else
{
    // Handle the error
    Console.WriteLine($"Authentication failed. Reason: {result.Error.Description}");
}
```

### CreateIdentityAsync

Creates a new user identity in the system. This is useful for user registration flows.

**Example: Creating a new identity**

```csharp
var newIdentity = new IdentityEnrollmentCredentials(
    email: "new.user@email.com",
    password: "a-strong-password",
    firstName: "New",
    lastName: "User"
);

var result = await identityClient.CreateIdentityAsync(newIdentity);

if (result.IsSuccess)
{
    Console.WriteLine("Identity created successfully!");
}
else
{
    Console.WriteLine($"Failed to create identity. Reason: {result.Error.Description}");
}
```

### InvalidateSessionAsync

Invalidates a user's session by revoking their refresh token. This is equivalent to a "logout" operation.

**Example: Logging a user out**

```csharp
var sessionToInvalidate = new SessionInvalidation("the-refresh-token-to-revoke");
var result = await identityClient.InvalidateSessionAsync(sessionToInvalidate);

if (result.IsSuccess)
{
    Console.WriteLine("Session invalidated successfully.");
}
else
{
    Console.WriteLine($"Logout failed. Reason: {result.Error.Description}");
}
```

## Predictable Error Handling

One of the key features of this SDK is its predictable, transparent error handling. Every method returns a `Result` object that contains an `Error` property when an operation fails.

Instead of relying on fragile string comparisons or magic values, you can check for specific, known errors using the static error classes provided in the `HttpsRichardy.Federation.Sdk.Errors` namespace. This makes your error-handling logic robust and easy to read.

For example, if authentication fails due to incorrect credentials, the `result.Error` will be equal to `AuthenticationErrors.InvalidCredentials`.

**Example: Handling a specific error**

```csharp
using HttpsRichardy.Federation.Sdk.Errors;

var credentials = new AuthenticationCredentials("user@email.com", "wrong-password");
var result = await identityClient.AuthenticateAsync(credentials);

if (result.IsFailure)
{
    if (result.Error == AuthenticationErrors.InvalidCredentials)
    {
        // Specific logic for invalid credentials
        Console.WriteLine("Invalid email or password. Please try again.");
    }
    else if (result.Error == AuthenticationErrors.UserNotFound)
    {
        // Handle the case where the user doesn't exist
        Console.WriteLine("No account found with that email address.");
    }
    else
    {
        // Generic error handling for unexpected issues
        Console.WriteLine($"An unexpected error occurred: {result.Error.Description}");
    }
}
```

This approach eliminates guesswork and allows you to build reliable application flows based on the exact type of error returned by the API.
