<template>
  <NuxtLayout>
    <NuxtPage />
    <DialogWrapper />
  </NuxtLayout>
</template>

<script setup>
import { DialogWrapper } from "vue3-promise-dialog";
import { provideUseId } from "@headlessui/vue";

provideUseId(() => useId().replace(/[-:]/, "_"));
</script>
