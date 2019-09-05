import Oidc from "oidc-client";
export const userManager = new Oidc.UserManager({
    authority: "http://localhost:5000",
    client_id: "vue-spa",
    redirect_uri: "http://localhost:8080/callback",
    response_type: "code",
    scope: "openid profile api1",
    post_logout_redirect_uri: "http://localhost:8080"
});