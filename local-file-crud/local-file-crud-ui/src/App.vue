<template>
  <Tree :value="nodes" @nodeExpand="onNodeExpand" :loading="loading"> </Tree>
</template>

<script>
import Tree from "primevue/tree";

export default {
  name: "App",
  data() {
    return {
      display: true,
      nodes: [],
    };
  },
  components: {
    Tree,
  },
  mounted() {
    this.getFilesInFolderNodes();
  },
  methods: {
    getFilesInFolderNodes() {
      this.GetFilesInFolderNodes();
    },
    GetFilesInFolderNodes() {
      fetch("https://localhost:44304/localfilecrud")
        .then((response) => response.json())
        .then((res) => (this.nodes = res));
    },
    onNodeExpand(node) {
      if (!node.children) {
        this.loading = true;
        fetch(
          `https://localhost:44304/localfilecrud?innerFolder=${node.fullPath}`
        )
          .then((data) => data.json())
          .then((res) => {
            node.children = res;
            this.loading = false;
          });
      }
    },
  },
};
</script>

<style>
</style>
