<template>
  <div>
    <b-container>
      <div class="mt-5">
        <b-row>
          <b-col>
            <b>Name</b>
          </b-col>
          <b-col>
            <b>Damage Cost</b>
          </b-col>
          <b-col>
            <b>Year</b>
          </b-col>
          <b-col>
            <b>Type</b>
          </b-col>
          <b-col></b-col>
        </b-row>

        <b-row class="mt-2" v-for="claim in claims" :key="claim.Id">
          <b-col>{{claim.name}}</b-col>
          <b-col>{{claim.damageCost}}</b-col>
          <b-col>{{claim.year}}</b-col>
          <b-col>{{claim.type}}</b-col>
          <b-col>
            <b-button @click="showDeleteConfirmDialog(claim.id)" size="sm" variant="danger">Delete</b-button>
          </b-col>
        </b-row>
        <b-row class="mt-4">
          <b-col>
            <b-input
              v-model="claimData.name"
              id="inline-form-input-name"
              class="mb-2 mr-sm-2 mb-sm-0"
              placeholder="Name"
            ></b-input>
          </b-col>
          <b-col>
            <b-input
              id="inline-form-input-name"
              class="mb-2 mr-sm-2 mb-sm-0"
              placeholder="Damage Cost"
              v-model="claimData.damageCost"
            ></b-input>
          </b-col>
          <b-col>
            <b-input
              v-model="claimData.year"
              id="inline-form-input-name"
              class="mb-2 mr-sm-2 mb-sm-0"
              placeholder="Year"
            ></b-input>
          </b-col>
          <b-col>
            <b-form-select
              id="inline-form-input-name"
              v-model="claimData.type"
              :options="claimTypes"
            ></b-form-select>
          </b-col>
          <b-col>
            <b-button @click="addClaim()" variant="info">Add</b-button>
          </b-col>
        </b-row>
      </div>
      <div>
        <div>
          <b-modal
            @hidden="resetSelectedClaimId()"
            @ok="deleteClaim()"
            v-model="modalShowConfirmDelete"
          >
            <p>Are you sure that you want to delete</p>
          </b-modal>
        </div>
      </div>
    </b-container>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "ClaimManagement",
  data() {
    return {
      claims: [],
      modalShowConfirmDelete: false,
      selectedClaimId: "",
      claimData: {
        name: "",
        damageCost: "",
        year: "",
        type: ""
      },
      claimTypes: [],
      baseUrl:
        "https://insuranceclaimhandlerapi20191123035338.azurewebsites.net/"
    };
  },
  created() {
    this.getClaimTypes();
    this.getClaims();
  },
  methods: {
    getClaims() {
      axios
        .get(this.baseUrl + "api/claim/all")
        .then(response => {
          this.claims = response.data;
        })
        .catch(error => {
          this.$bvToast.toast(` ${JSON.stringify(error.response.data)}`, {
            title: "error occurred while fetching claims",
            autoHideDelay: 2000,
            variant: "danger",
            solid: true
          });
        });
    },

    getClaimTypes() {
      axios
        .get(this.baseUrl + "api/claim/types")
        .then(response => {
          this.claimTypes = response.data;
        })
        .catch(error => {
          this.$bvToast.toast(` ${JSON.stringify(error.response.data)}`, {
            title: "error",
            autoHideDelay: 2000,
            variant: "danger",
            solid: true
          });
        });
    },

    deleteClaim() {
      if (!this.selectedClaimId) {
        this.$bvToast.toast("Please select claim to delete", {
          title: "warning",
          autoHideDelay: 2000,
          variant: "warning",
          solid: true
        });
        return;
      }
      axios
        .delete(this.baseUrl + "api/claim/" + this.selectedClaimId)
        .then(response => {
          if (response) {
            this.$bvToast.toast("claim deleted successfully", {
              title: "success",
              autoHideDelay: 2000,
              variant: "success",
              solid: true
            });
            this.getClaims();
          }
        })
        .catch(error => {
          this.$bvToast.toast(` ${JSON.stringify(error.response.data)}`, {
            title: "error",
            autoHideDelay: 2000,
            variant: "danger",
            solid: true
          });
        });
    },

    addClaim() {
      if (
        !this.claimData ||
        !this.claimData.name ||
        !this.claimData.damageCost ||
        !this.claimData.year ||
        !this.claimData.type
      ) {
        this.$bvToast.toast("Please provide all the input data", {
          title: "warning",
          autoHideDelay: 2000,
          variant: "warning",
          solid: true
        });
        return;
      }
      axios
        .post(this.baseUrl + "api/claim/add", this.claimData)
        .then(response => {
          if (response) {
            this.$bvToast.toast("claim added successfully", {
              title: "success",
              autoHideDelay: 2000,
              variant: "success",
              solid: true
            });
            this.selectedClaimId = "";
            this.resetClaimData();
            this.getClaims();
          }
        })
        .catch(error => {
          this.$bvToast.toast(` ${JSON.stringify(error.response.data)}`, {
            title: "error",
            autoHideDelay: 2000,
            variant: "danger",
            solid: true
          });
        });
    },

    resetClaimData() {
      this.claimData = {
        name: "",
        damageCost: "",
        year: "",
        type: ""
      };
    },

    showDeleteConfirmDialog(id) {
      this.selectedClaimId = id;
      this.modalShowConfirmDelete = true;
    },

    resetSelectedClaimId() {
      this.selectedClaimId = "";
      this.modalShowConfirmDelete = false;
    }
  }
};
</script>
