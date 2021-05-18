<template>
  <Checkbox v-model="showFiles" :binary="true" />
  <label>Show Files</label>
  <div class="flex flex-row">
    <Tree
      class="w-full"
      :value="nodes"
      @nodeExpand="onNodeExpand"
      :loading="loading"
      expandedKey="fullPath"
      selectionMode="multiple"
      v-model:selectionKeys="selectedDocuments"
      @node-select="onNodeSelect"
      :metaKeySelection="false"
    >
    </Tree>
    <div class="w-full">
      <Button @click="sendEmailWithFiles">Send Email</Button>
      <pre>{{ JSON.stringify(selectedDocuments, undefined, 2) }}</pre>
      <pre>{{ JSON.stringify(selectedDocumentsWithData, undefined, 2) }}</pre>
      <div class="flex flex-col">
        results:
        <pre>{{ JSON.stringify(postedResult, undefined, 2) }}</pre>
      </div>
    </div>
  </div>
</template>

<script>
import Checkbox from "primevue/checkbox";
import Tree from "primevue/tree";
import Button from "primevue/button";

export default {
  name: "App",
  data() {
    return {
      display: true,
      showFiles: true,
      nodes: [],
      selectedDocuments: [],
      selectedDocumentsWithData: [],
      postedResult: [],
    };
  },
  components: {
    Tree,
    Button,
    Checkbox,
  },
  mounted() {
    this.getFilesInFolderNodes();
  },
  watch: {
    showFiles: function () {
      this.getFilesInFolderNodes();
    },
  },
  methods: {
    sendEmailWithFiles() {
      this.selectedDocumentsWithData = Object.keys(this.selectedDocuments);

      fetch("https://localhost:44304/localfilecrud/sendFiles", {
        method: "post",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
        //make sure to serialize your JSON body
        body: JSON.stringify([...this.selectedDocumentsWithData]),
      })
        .then((response) => response.json())
        .then((response) => (this.postedResult = response));
    },
    getFilesInFolderNodes() {
      this.GetFilesInFolderNodes();
    },
    GetFilesInFolderNodes() {
      fetch(`https://localhost:44304/localfilecrud?showFiles=${this.showFiles}`)
        .then((response) => response.json())
        .then(
          (res) =>
            (this.nodes = res.map((i) => {
              return {
                label: i.label,
                leaf: i.leaf,
                selectable: i.selectable,
                key: i.fullPath,
                fullPath: i.fullPath,
              };
            }))
        );
    },
    onNodeExpand(node) {
      if (!node.children) {
        this.loading = true;
        fetch(
          `https://localhost:44304/localfilecrud?innerFolder=${node.fullPath}&showFiles=${this.showFiles}`
        )
          .then((data) => data.json())
          .then((res) => {
            node.children = res.map((i) => {
              return {
                label: i.label,
                leaf: i.leaf,
                selectable: i.selectable,
                key: i.fullPath,
                fullPath: i.fullPath,
              };
            });
            this.loading = false;
          });
      }
    },
  },
};
</script>

<style>
.flex {
  display: flex;
}

.flex-col {
  flex-direction: column;
}
.flex-row {
  flex-direction: row;
}

.w-full {
  width: 100%;
}
</style>
