<template>
    <div>
        轉址中
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Oidc from "oidc-client";
import axios from "axios";

@Component({})
export default class Callback extends Vue {
    async created() {
        try {
            await new Oidc.UserManager({
                response_mode: "query"
            }).signinRedirectCallback();
            this.$router.push("/");
        } catch (error) {
            console.error(error);
        }
    }
}
</script>
