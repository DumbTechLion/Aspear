<template>
  <Combobox as="div" v-model="selectedValues" @update:modelValue="query = ''" multiple>
    <ComboboxLabel class="block text-sm/6 font-medium text-gray-900">Assigned to</ComboboxLabel>
    <div class="relative mt-2">
    <ComboboxInput class="w-full rounded-md border-0 bg-white py-1.5 pl-3 pr-10 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm/6" @change="query = $event.target.value" @blur="query = ''" :display-value="(person) => person?.name" />
    <ComboboxButton class="absolute inset-y-0 right-0 flex items-center rounded-r-md px-2 focus:outline-none">
        <Icon name="mdi:chevron-down" class="size-5 text-gray-400" aria-hidden="true" />
    </ComboboxButton>

    <ComboboxOptions v-if="filteredOptions.length > 0" class="absolute z-10 mt-1 max-h-60 w-full overflow-auto rounded-md bg-white py-1 text-base shadow-lg ring-1 ring-black/5 focus:outline-none sm:text-sm">
        <ComboboxOption v-for="option in filteredOptions" :key="option.value" :value="option.value" as="template" v-slot="{ active, selected }">
        <li :class="['relative cursor-default select-none py-2 pl-8 pr-4', active ? 'bg-indigo-600 text-white outline-none' : 'text-gray-900']">
            <span :class="['block truncate', selected && 'font-semibold']">
            {{ option.name }}
            </span>

            <span v-if="selected" :class="['absolute inset-y-0 left-0 flex items-center pl-1.5', active ? 'text-white' : 'text-indigo-600']">
            <Icon name="mdi:check" class="size-5" aria-hidden="true" />
            </span>
        </li>
        </ComboboxOption>
    </ComboboxOptions>
    </div>
  </Combobox>
</template>

<script lang="ts" setup>
import { computed, ref } from 'vue';
import {
Combobox,
ComboboxButton,
ComboboxInput,
ComboboxLabel,
ComboboxOption,
ComboboxOptions,
} from '@headlessui/vue';

interface Option {
  name: string;
  value: string | number;
}
interface Props {
  label: string;
  options: Option[];
  selectedValues: (string | number)[];
}
const props = defineProps<Props>();

const query = ref('')
const selectedValues = ref([])
const filteredOptions = computed(() =>
  query.value === ''
    ? props.options
    : props.options.filter((option) => {
        return option.name.toLowerCase().includes(query.value.toLowerCase())
    }),
  )
</script>