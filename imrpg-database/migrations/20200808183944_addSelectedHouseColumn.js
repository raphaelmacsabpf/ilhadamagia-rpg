
exports.up = function(knex) {
    return knex.schema.alterTable('imtb_account', function(t) {
        t.integer('SelectedHouse');
    });
};

exports.down = function(knex) {
  return knex.schema.alterTable('imtb_account', function(t) {
    t.dropColumn('SelectedHouse');
  });
};
