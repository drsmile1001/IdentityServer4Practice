<template>
    <div class="login">
        <h1>登入</h1>
        <label>帳號</label>
        <input v-model="account"/>
        <label>密碼</label>
        <input v-model="password"/>
        <button @click="login">登入</button>
        <div>{{returnUrl}}</div>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import axios from "axios";

@Component({})
export default class Login extends Vue {
    account="";
    password="";
    returnUrl:string|undefined="";
    created(){
        this.returnUrl = this.getQueryVariable(window.location.search.substring(1),"ReturnUrl");
    }

    getQueryVariable(query:string,variable: string) {
        const vars = query.split("&");
        for (let i = 0; i < vars.length; i++) {
            let pair = vars[i].split("=");
            if (decodeURIComponent(pair[0]) == variable) {
                return decodeURIComponent(pair[1]);
            }
        }
    }

    async login() {
        
        const response = await axios.post("http://localhost:5000/api/authenticate",{
            userName:this.account,
            password:this.password,
            returnUrl:this.returnUrl
        },{
            withCredentials:true
        })
        console.log(response);
        if(response){
            console.log(response.data.redirectUrl);
            window.location = response.data.redirectUrl;
        }
    }
}
</script>
