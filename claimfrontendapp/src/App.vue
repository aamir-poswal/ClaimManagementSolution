<template>
  <div id="app">
    <b-navbar toggleable="lg" type="dark" variant="info">
      <b-navbar-brand href="#">Insurance Claim Management</b-navbar-brand>

      <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

      <b-collapse id="nav-collapse" is-nav>
        <!-- Right aligned nav items -->
        <b-navbar-nav class="ml-auto">
          <img alt="Vue logo" src />
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
    <ClaimManagement />
  </div>
</template>

<script>
import ClaimManagement from "./components/ClaimManagement.vue";

export default {
  name: "app",
  components: {
    ClaimManagement
  }
};
</script>
<style></style>
