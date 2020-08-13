
exports.up = function(knex) {
    return knex.schema.alterTable('imtb_account', function(t) {
        t.integer('OrgId').notNull().defaultTo(0);
    });
};

exports.down = function(knex) {
  return knex.schema.alterTable('imtb_account', function(t) {
    t.dropColumn('OrgId');
  });
};
