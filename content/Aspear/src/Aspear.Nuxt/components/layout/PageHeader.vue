<template>
  <div v-if="meta.layoutMetadata" class="lg:flex lg:items-center lg:justify-between">
    <div class="min-w-0 flex-1">
      <nav class="flex" aria-label="Breadcrumb">
        <ol role="list" class="flex items-center space-x-4">
          <li v-for="(breadcrumb, index) in meta.layoutMetadata.breadcrumbs">
            <div class="flex items-center">
              <ChevronRightIcon
                v-if="index > 0"
                class="h-5 w-5 flex-shrink-0 text-gray-400 mr-4"
                aria-hidden="true"
              />
              <a
                class="text-sm font-medium text-gray-500 hover:text-gray-700"
                :href="breadcrumb.href"
                @click="breadcrumb.onClick"
                >{{ breadcrumb.title }}</a
              >
            </div>
          </li>
        </ol>
      </nav>
      <h2
        class="flex items-center mt-2 text-2xl font-bold leading-7 text-gray-900 sm:text-3xl sm:tracking-tight"
      >
        <Icon
          v-if="meta.layoutMetadata.titleIcon"
          :name="meta.layoutMetadata.titleIcon"
          class="mr-1.5 h-8 w-8 flex-shrink-0 text-gray-900"
          aria-hidden="true"
        />
        {{ meta.layoutMetadata.title }}
      </h2>
      <div
        v-if="meta.layoutMetadata.subElements"
        class="mt-1 flex flex-col sm:mt-0 sm:flex-row sm:flex-wrap sm:space-x-6"
      >
        <div
          v-for="subElement in meta.layoutMetadata.subElements"
          class="mt-2 flex items-center text-sm text-gray-500"
        >
          <Icon
            :name="subElement.icon"
            class="mr-1.5 h-5 w-5 flex-shrink-0 text-gray-400"
            aria-hidden="true"
          />
          {{ subElement.title }}
        </div>
      </div>
    </div>
    <div class="mt-5 flex lg:ml-4 lg:mt-0">
      <a
        v-for="actionButton in meta.layoutMetadata.actions"
        :key="actionButton.href"
        class="mr-3"
        :class="[actionButton.alwaysVisible ? '' : 'hidden sm:inline-flex']"
        :href="actionButton.href"
        @click="actionButton.onClick">
        <button
          type="button"
          :data-variant="actionButton.variant ?? 'outline'"
          :data-color="actionButton.color"
        >
          <Icon :name="actionButton.icon" class="mr-1.5" aria-hidden="true" />
          {{ actionButton.title }}
        </button>
      </a>

      <!-- Dropdown -->
      <Menu as="div" class="relative sm:hidden">
        <MenuButton
          class="inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:ring-gray-400"
        >
          {{ t("menu.more") }}
          <Icon
            name="mdi:chevron-down"
            class="ml-1.5 -mr-1.5"
            aria-hidden="true"
          />
        </MenuButton>

        <transition
          enter-active-class="transition ease-out duration-200"
          enter-from-class="transform opacity-0 scale-95"
          enter-to-class="transform opacity-100 scale-100"
          leave-active-class="transition ease-in duration-75"
          leave-from-class="transform opacity-100 scale-100"
          leave-to-class="transform opacity-0 scale-95"
        >
          <MenuItems
            class="absolute right-0 z-10 -mr-1 mt-2 w-48 origin-top-right rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none"
          >
            <MenuItem
              v-for="actionButton in meta.layoutMetadata.actions.filter(
                (b) => !b.alwaysVisible
              )"
              v-slot="{ active }"
            >
              <a
                :key="actionButton.href"
                :href="actionButton.href"
                @click="actionButton.onClick"
                class="block px-4 py-2 text-sm"
              >
                <button :data-variant="actionButton.variant" :data-color="actionButton.color">
                  {{ actionButton.title }}
                </button>
              </a>
            </MenuItem>
          </MenuItems>
        </transition>
      </Menu>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRoute } from "vue-router";
import {
  ChevronRightIcon,
} from "@heroicons/vue/20/solid";
import { Menu, MenuButton, MenuItem, MenuItems } from "@headlessui/vue";

const { meta } = useRoute();
const { t } = useI18n();

</script>
