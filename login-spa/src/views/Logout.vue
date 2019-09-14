<template>
    <div class="logout">
        <div id="logout_iframe">
        </div>
        {{msg}}
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import axios from "axios";

@Component({})
export default class Logout extends Vue {
    msg = "";
    created() {
        this.logMeOut();
    }
    async logMeOut() {
        const query = window.location.search;
        const logoutIdQuery =
            query && query.toLowerCase().indexOf("?logoutid=") == 0 && query;

        const response = await fetch(
            "http://localhost:5000/api/authenticate/logout" + logoutIdQuery,
            {
                credentials: "include"
            }
        );

        const data = await response.json();

        console.log(data);

        if (data.signOutIFrameUrl) {
            var iframe = document.createElement("iframe");
            iframe.width = 0;
            iframe.height = 0;
            iframe.class = "signout";
            iframe.src = data.signOutIFrameUrl;
            document.getElementById("logout_iframe").appendChild(iframe);
        }

        if (data.postLogoutRedirectUri) {
            window.location = data.postLogoutRedirectUri;
        } else {
            this.msg = "您可以關閉視窗了";
        }
    }
}
</script>
