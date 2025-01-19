<template>
  <div
    class="h-8 w-8 rounded-full bg-gray-200 flex items-center justify-center text-xs"
  >
    {{ initials }}
  </div>
</template>

<script lang="ts" setup>
const { name } = defineProps({
  name: {
    type: String,
    required: true,
  },
});
const initials = computed(() => {
  const names = name.split(" ");
  return names.reduce((acc, n) => acc + n[0], "");
});
</script>

<style></style>
