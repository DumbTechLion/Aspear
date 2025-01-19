<template>
  <div class="flex flex-col w-full">
    <div class="px-4 sm:px-0">
      <h3>{{ t("cvssStats.title", { days: 31 }) }}</h3>
      <p class="mb-2 text-gray-500 text-sm">{{ t("cvssStats.subtitle", { count: totalVulnerabilities }) }}</p>
    </div>
    <div
      class="flex items-center justify-center self-center w-[550px] h-[330px]"
    >
      <ClientOnly fallbackTag="span">
        <Pie :data="pieData" :options="options" />
        <template #fallback>
          <p>Loading...</p>
        </template>
      </ClientOnly>
    </div>
  </div>
</template>

<script setup>
import { Pie } from "vue-chartjs";
const { t } = useI18n();
const props = defineProps({
  departmentId: {
    type: String,
    required: false,
  },
  organizationId: {
    type: String,
    required: false,
  },
});
const { data } = await useAuthApi("/Customer/Vulnerability/cvssstats", {
  query: { departmentId: props.departmentId, organizationId: props.organizationId },
  server: false,
});

const pieData = computed(() => ({
  labels: ["CRITICAL", "HIGH", "MEDIUM", "LOW", "NONE"],
  datasets: [
    {
      label: "CVSS3 Severity",
      backgroundColor: [
        "rgb(40, 20, 20)",
        "rgb(217, 83, 79)",
        "rgb(236, 151, 31)",
        "rgb(242, 204, 12)",
        "rgb(217, 217, 217)",
      ],
      data: [
        data.value?.countPerCvss3Severity?.CRITICAL ?? 0,
        data.value?.countPerCvss3Severity?.HIGH ?? 0,
        data.value?.countPerCvss3Severity?.MEDIUM ?? 0,
        data.value?.countPerCvss3Severity?.LOW ?? 0,
        data.value?.countPerCvss3Severity?.NONE ?? 0,
      ],
      labels: ["CRITICAL", "HIGH", "MEDIUM", "LOW", "NONE"],
    },
  ],
}));
const totalVulnerabilities = computed(() =>
  Object.values(data.value?.countPerCvss3Severity ?? {}).reduce(
    (acc, value) => acc + value,
    0
  )
);

const options = ref({
  responsive: true,
  aspectRatio: 5 / 3,
  layout: {
    padding: {
      left: 128,
      right: 128,
      top: 32,
      bottom: 32,
    },
  },
  plugins: {
    legend: false,
    datalabels: {
      anchor: "end",
      align: "end",
      color: "black",
      display: "auto",
      font: {
        weight: 500,
      },
      padding: 6,
      formatter: function (value, context) {
        return context.dataset.labels[context.dataIndex] + " - " + value;
      },
    },
  },
});
</script>
