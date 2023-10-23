<template>
  <!-- <i-frame :src="url" /> -->
  <div> 
    <el-row>
      <el-col :sm="24" :lg="24">
        <el-result icon="info" title="提示" subTitle="功能完善中...">
          <template slot="extra">
            <el-button type="primary" size="medium" @click="handleClose">返回</el-button>
          </template>
        </el-result>
      </el-col>
    </el-row>
  </div>
</template>
<script>
import iFrame from "@/components/iFrame/index";
export default {
  name: "Druid",
  components: { iFrame },
  data() {
    return {
      url: process.env.VUE_APP_BASE_API + "/druid/login.html"
    };
  },
  methods: {    
    // 返回按钮
    handleClose() {      
      const obj = { path: "/system/user" };
      this.$tab.closeOpenPage(obj);

      this.$router.push({ path: '/' })
    },
  }
};
</script>
