<template>
  <ModalDialog
    :header="props.header"
    :confirmLabel="props.confirmLabel"
    :cancelLabel="props.cancelLabel"
    @close="(val) => result = val">
    <template #side>
      <div class="mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full bg-red-100 sm:mx-0 sm:h-10 sm:w-10">
        <Icon name="mdi:warning-outline" class="h-6 w-6 text-red-600" aria-hidden="true" />
      </div>
    </template>
    <template #default>
      <p class="text-sm text-gray-500">{{ text }}</p>
    </template>
  </ModalDialog>
</template>

<script setup lang="ts">
interface Props {
  header: string;
  text: string;
  confirmLabel?: string;
  cancelLabel?: string;
}

const props = defineProps<Props> ();
const result = ref(false);

// Return the result of the dialog to the openDialog call
defineExpose({
  returnValue: () => {
    return result.value
  },
});
</script>