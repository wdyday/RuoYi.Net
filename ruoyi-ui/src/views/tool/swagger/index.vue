<template>
  <i-frame :src="url" />
</template>
<script>
import iFrame from "@/components/iFrame/index";
export default {
  name: "Swagger",
  components: { iFrame },
  data() {
    return {
      // url: process.env.VUE_APP_BASE_API + "/swagger-ui/index.html",
      url: process.env.VUE_APP_API_URL + "/swagger-ui/index.html",
      url2: process.env.VUE_APP_API_URL
    };
  }
};
</script>
