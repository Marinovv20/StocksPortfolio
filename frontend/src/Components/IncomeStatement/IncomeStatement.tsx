import React, { useEffect, useState } from "react";
import { CompanyIncomeStatement, CompanyKeyMetrics, CompanyKeyRatios } from "../../company";
import { useOutletContext } from "react-router";
import { get } from "http";
import { getIncomeStatement } from "../../api";
import Table from "../Table/Table";
import Spinner from "../Spinner/Spiner";
import { formatLargeMonetaryNumber, formatRatio } from "../../Helpers/NumberFormating";


type Props = {};

const configs = [
 {
    label: "Date",
    render: (company: CompanyIncomeStatement) => company.date,
  },
  {
    label: "Revenue",
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.revenue),
  },
  {
    label: "Cost Of Revenue",
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.costOfRevenue),
  },
  {
    label: "Depreciation",
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.depreciationAndAmortization),
  },
  {
    label: "Operating Income",
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.operatingIncome),
  },
  {
    label: "Income Before Taxes",
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.incomeBeforeTax),
  },
  {
    label: "Net Income",
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.netIncome),
  },
  {
    label: "Earnings Per Share",
    render: (company: CompanyIncomeStatement) => formatLargeMonetaryNumber(company.eps),
  },
  {
    label: "Income Before Taxes",
    render: (company: CompanyIncomeStatement) =>
      formatLargeMonetaryNumber(company.incomeBeforeTax),
  },
];

const IncomeStatement = (props: Props) => {
  const ticker = useOutletContext<string>();
  const [incomeStatement, setIncomeStatement] = useState<CompanyIncomeStatement[]>();
  useEffect(() => {
  const incomeStatementFetch = async () => {
    if (!ticker) return;
      try {
      const result = await getIncomeStatement(ticker);
      if (result && (result as any).data) setIncomeStatement((result as any).data);
      else setIncomeStatement([]); 
      } catch (e) {
      console.error("Income statement fetch failed", e);
      setIncomeStatement([]);
    }
  };
  incomeStatementFetch();
  }, [ticker]);
   return (
    <>
      {incomeStatement ? (
        <Table config={configs} data={incomeStatement} />
      ) : (
        <Spinner />
      )}
    </>
  );
};

export default IncomeStatement;