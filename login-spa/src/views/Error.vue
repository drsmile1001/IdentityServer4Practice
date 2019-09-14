<template>
    <div class="login">
        <h1>錯誤</h1>
        <div>{{errorMessage}}</div>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import axios from "axios";

@Component({})
export default class Error extends Vue {
    errorMessage = "";
    async created() {
        const query = window.location.search;
        const errorIdQuery =
            query && query.toLowerCase().indexOf("?errorId=") === 0 && query;
        const response = await axios.get(
            `http://localhost:5000/api/authenticate/error${errorIdQuery}`,
            {
                withCredentials: true
            }
        );
        this.errorMessage = response.data;
    }
}
</script>
