import Oidc from "oidc-client";
export const userManager = new Oidc.UserManager({
    authority: "http://localhost:5000",
    client_id: "login-spa",
    redirect_uri: "http://localhost:8082/callback",
    response_type: "code",
    scope: "openid profile api1 IdentityServerApi",
    post_logout_redirect_uri: "http://localhost:8082"
});