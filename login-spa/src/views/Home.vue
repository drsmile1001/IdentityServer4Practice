<template>
    <div class="home">
        <button @click="login">登入</button>
        <button @click="api">呼叫被保護的API</button>
        <button @click="apiOnIdentityServer">呼叫IdentityServer上的API</button>
        <button @click="checkAdmin">檢查是否是Admin</button>
        <button @click="logout">登出</button>
        <pre>
            {{logMessages}}
        </pre>

    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { userManager } from "@/userManager.ts";
import axios from "axios";

@Component({})
export default class Home extends Vue {
    logMessages = "";
    created() {
        userManager.getUser().then(user => {
            if (user) {
                this.log("User logged in", user.profile);
            } else {
                this.log("User not logged in");
            }
        });
    }
    log(...messages: any[]) {
        messages.forEach(msg => {
            if (msg instanceof Error) {
                msg = `Error:${msg.message}`;
            } else if (typeof msg !== "string") {
                msg = JSON.stringify(msg, null, 2);
            }
            this.logMessages = `${this.logMessages}\r\n${msg}`;
        });
    }
    login() {
        userManager.signinRedirect();
    }
    async callApi(url: string) {
        const user = await userManager.getUser();
        try {
            const response = await axios.get(
                url,
                {
                    headers: {
                        Authorization: `Bearer ${user && user.access_token}`
                    }
                }
            );
            this.log(response.status, response.data);
        } catch (error) {
            this.log(error);
        }
    }
    async api() {
        this.callApi("http://localhost:5002/api/Values")
    }
    async logout() {
        userManager.signoutRedirect();
    }
    async apiOnIdentityServer() {
        this.callApi("http://localhost:5000/api/test/get")
    }
    async checkAdmin() {
        this.callApi("http://localhost:5000/api/test/admin")
    }
}
</script>
